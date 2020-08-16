START TRANSACTION;

  CREATE TABLE tenant
  (
    id int PRIMARY KEY NOT NULL
  );

  CREATE TABLE document
  (
    tenant_id int NOT NULL REFERENCES tenant,
    id uuid NOT NULL,
    slug text NOT NULL,

    created timestamp NOT NULL,

    title text NOT NULL,
    description text NULL,

    blob_name text NOT NULL,
    analysis_blob_id uuid NULL,

    PRIMARY KEY (tenant_id, id),
    CONSTRAINT document_slug_key UNIQUE(tenant_id, slug)
  );

  CREATE TABLE site_user
  (
    tenant_id int NOT NULL REFERENCES tenant,
    id uuid NOT NULL,
    email_address text NOT NULL,
    PRIMARY KEY (tenant_id, id)
  );

  CREATE TABLE document_access
  (
    tenant_id int NOT NULL REFERENCES tenant,
    document_id uuid NOT NULL,
    site_user_id uuid NULL,
    access_date timestamp NOT NULL,
    access_type text NOT NULL,
    FOREIGN KEY (tenant_id, document_id) REFERENCES document (tenant_id, id),
    FOREIGN KEY (tenant_id, site_user_id) REFERENCES site_user (tenant_id, id)
  );

  INSERT INTO tenant (id)
  VALUES (1),
         (2),
         (3),
         (4);

  INSERT INTO site_user (tenant_id, id, email_address)
  VALUES (1, '43954e2f-bc63-4025-bf72-52e8da5c898b'::uuid, 'test@example.com');

  INSERT INTO site_user (tenant_id, id, email_address)
  VALUES (2, '43954e2f-bc63-4025-bf72-52e8da5c898b'::uuid, 'test@example.com');

COMMIT TRANSACTION;
