START TRANSACTION;

  CREATE TABLE site_user
  (
    id uuid NOT NULL PRIMARY KEY,
    name text NOT NULL
  );

  CREATE TABLE bug
  (
    id uuid PRIMARY KEY NOT NULL,
    slug text NOT NULL,

    created timestamptz NOT NULL,

    title text NOT NULL,
    description text NULL,

    active_user_id uuid NULL REFERENCES site_user
  );

COMMIT TRANSACTION;
