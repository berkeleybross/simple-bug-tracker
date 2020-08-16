START TRANSACTION;

  CREATE TABLE site_user
  (
    id uuid NOT NULL PRIMARY KEY,
    name text NOT NULL
  );

  CREATE TABLE bug
  (
    id int PRIMARY KEY NOT NULL GENERATED ALWAYS AS IDENTITY,
    created timestamptz NOT NULL,

    title text NOT NULL,
    description text NULL,
    status int NOT NULL,

    active_user_id uuid NULL REFERENCES site_user
  );

COMMIT TRANSACTION;
