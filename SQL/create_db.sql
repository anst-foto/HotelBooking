-- CREATE DATABASE hotel_db;

-- <LOG>
CREATE SCHEMA log;
SET search_path = "log";

CREATE TYPE type_event AS ENUM ('CREATE', 'DELETE');
CREATE TABLE table_ddl_logs
(
    id           SERIAL     NOT NULL PRIMARY KEY,
    date_action  DATE       NOT NULL DEFAULT current_date,
    time_action  TIME       NOT NULL DEFAULT current_time,
    client_ip    inet       NULL,
    user_name    TEXT       NOT NULL,
    event_type   type_event NULL,
    object_type  TEXT       NULL,
    schema_name  TEXT       NULL,
    object_name  TEXT       NOT NULL,
    command      TEXT       NULL,
    command_tag  TEXT       NULL,
    command_text TEXT       NULL
);
CREATE OR REPLACE FUNCTION function_ddl_logger()
    RETURNS event_trigger
    LANGUAGE plpgsql
AS
$$
BEGIN
    IF tg_event = 'sql_drop' THEN
        INSERT INTO log.table_ddl_logs(client_ip, user_name, event_type, object_type, schema_name, object_name, command,
                                       command_tag, command_text)
        SELECT inet_client_addr(),
               current_user,
               'DELETE',
               object_type,
               schema_name,
               object_identity,
               tg_tag,
               NULL,
               current_query()
        FROM pg_event_trigger_dropped_objects()
        WHERE schema_name NOT IN ('pg_temp', 'pg_toast');
    ELSE
        INSERT INTO log.table_ddl_logs(client_ip, user_name, event_type, object_type, schema_name, object_name, command,
                                       command_tag, command_text)
        SELECT inet_client_addr(),
               current_user,
               'CREATE',
               object_type,
               schema_name,
               object_identity,
               tg_tag,
               command_tag,
               current_query()
        FROM pg_event_trigger_ddl_commands()
        WHERE schema_name NOT IN ('pg_temp', 'pg_toast');
    END IF;
END;
$$;

CREATE EVENT TRIGGER trigger_ddl_log_create
    ON ddl_command_end
EXECUTE FUNCTION log.function_ddl_logger();

CREATE EVENT TRIGGER trigger_ddl_log_drop
    ON sql_drop
EXECUTE FUNCTION log.function_ddl_logger();

CREATE TYPE type_operation AS ENUM ('INSERT', 'UPDATE', 'DELETE', 'TRUNCATE');
CREATE TABLE table_dml_logs
(
    id             BIGSERIAL      NOT NULL PRIMARY KEY,
    schema_name    TEXT           NOT NULL,
    table_name     TEXT           NOT NULL,
    operation_type type_operation NOT NULL,
    date_operation DATE           NOT NULL DEFAULT current_date,
    time_operation TIME           NOT NULL DEFAULT current_time,
    user_name      TEXT           NOT NULL,
    old_data       jsonb          NULL,
    new_data       jsonb          NULL
);

CREATE OR REPLACE FUNCTION function_dml_logger()
    RETURNS trigger
    LANGUAGE plpgsql
AS
$$
BEGIN
    IF (tg_op = 'INSERT') THEN
        INSERT INTO log.table_dml_logs(schema_name, table_name, operation_type, user_name, old_data, new_data)
        VALUES (tg_table_schema, tg_table_name, 'INSERT', current_user, NULL, to_jsonb(NEW));
    ELSIF (tg_op = 'UPDATE') THEN
        INSERT INTO log.table_dml_logs(schema_name, table_name, operation_type, user_name, old_data, new_data)
        VALUES (tg_table_schema, tg_table_name, 'UPDATE', current_user, to_jsonb(OLD), to_jsonb(NEW));
    ELSIF (tg_op = 'DELETE') THEN
        INSERT INTO log.table_dml_logs(schema_name, table_name, operation_type, user_name, old_data, new_data)
        VALUES (tg_table_schema, tg_table_name, 'DELETE', current_user, to_jsonb(OLD), NULL);
    ELSIF (tg_op = 'TRUNCATE') THEN
        INSERT INTO log.table_dml_logs(schema_name, table_name, operation_type, user_name, old_data, new_data)
        VALUES (tg_table_schema, tg_table_name, 'TRUNCATE', current_user, NULL, NULL);
    END IF;
    RETURN NULL;
END;
$$;
-- </LOG>
    
CREATE SCHEMA test;
SET search_path='test';

-- <TABLES>
CREATE TABLE table_clients (
    id SERIAL PRIMARY KEY,
    last_name TEXT NOT NULL,
    first_name TEXT NOT NULL,
    patronymic TEXT NULL,
    phone TEXT NOT NULL,
    email TEXT NOT NULL,
    date_of_birth DATE NOT NULL
);

CREATE TABLE table_rooms (
    id SERIAL PRIMARY KEY,
    number TEXT NOT NULL,
    is_deleted BOOLEAN NOT NULL DEFAULT FALSE
);

CREATE TABLE table_reservations (
    id SERIAL PRIMARY KEY,
    client_id INTEGER NOT NULL,
    room_id INTEGER NOT NULL,
    arrival_date DATE NOT NULL,
    departure_date DATE NOT NULL CHECK ( departure_date > arrival_date ),
    is_cancelled BOOLEAN NOT NULL DEFAULT FALSE,
    FOREIGN KEY (client_id) REFERENCES table_clients(id) 
        ON UPDATE NO ACTION 
        ON DELETE NO ACTION,
    FOREIGN KEY (room_id) REFERENCES table_rooms(id)
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);
-- </TABLES>

-- <TRIGGERS>
-- SET search_path='test';

CREATE TRIGGER trigger_dml_logger_for_table_clients
    AFTER INSERT OR UPDATE OR DELETE
    ON table_clients
    FOR EACH ROW
EXECUTE FUNCTION log.function_dml_logger();
CREATE TRIGGER trigger_truncate_logger_for_table_clients
    AFTER TRUNCATE
    ON table_clients
    FOR EACH STATEMENT
EXECUTE FUNCTION log.function_dml_logger();

CREATE TRIGGER trigger_dml_logger_for_table_rooms
    AFTER INSERT OR UPDATE OR DELETE
    ON table_rooms
    FOR EACH ROW
EXECUTE FUNCTION log.function_dml_logger();
CREATE TRIGGER trigger_truncate_logger_for_table_rooms
    AFTER TRUNCATE
    ON table_rooms
    FOR EACH STATEMENT
EXECUTE FUNCTION log.function_dml_logger();

CREATE TRIGGER trigger_dml_logger_for_table_reservations
    AFTER INSERT OR UPDATE OR DELETE
    ON table_reservations
    FOR EACH ROW
EXECUTE FUNCTION log.function_dml_logger();
CREATE TRIGGER trigger_truncate_logger_for_table_reservations
    AFTER TRUNCATE
    ON table_reservations
    FOR EACH STATEMENT
EXECUTE FUNCTION log.function_dml_logger();
    
-- </TRIGGERS>

-- <VIEWS>
CREATE VIEW view_reservations AS
    SELECT table_reservations.id AS reservation_id,
           table_reservations.arrival_date AS arrival_date,
           table_reservations.departure_date AS departure_date,
           table_clients.id AS client_id,
           table_clients.last_name AS last_name,
           table_clients.first_name AS first_name,
           table_clients.patronymic AS patronymic,
           table_clients.phone AS phone,
           table_clients.email AS email,
           table_clients.date_of_birth AS date_of_birth,
           table_rooms.id AS room_id,
           table_rooms.number AS room_number
    FROM table_reservations
        JOIN table_clients 
            ON table_reservations.client_id = table_clients.id
        JOIN table_rooms 
            ON table_reservations.room_id = table_rooms.id;
-- </VIEWS>

-- <TEST_DATA>
INSERT INTO table_clients(last_name, first_name, patronymic, phone, email, date_of_birth) 
VALUES ('Иванов', 'Иван', 'Иванович', '+79111111111', 'ivanov@mail.ru', '1990-01-01');
INSERT INTO table_rooms(number) 
VALUES ('101'),
       ('102');
INSERT INTO table_reservations(client_id, room_id, arrival_date, departure_date)
VALUES (1, 1, '2020-01-01', '2020-01-03');
-- </TEST_DATA>