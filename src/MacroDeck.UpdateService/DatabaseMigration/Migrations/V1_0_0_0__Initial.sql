CREATE SCHEMA IF NOT EXISTS updateservice;
ALTER SCHEMA updateservice OWNER TO updateservice;

CREATE SEQUENCE updateservice.version_id_seq START WITH 1 INCREMENT BY 1;
ALTER SEQUENCE updateservice.version_id_seq OWNER TO updateservice;
CREATE TABLE updateservice.version
(
    v_id                     INTEGER DEFAULT nextval('updateservice.version_id_seq') PRIMARY KEY,
    v_version_string         TEXT                  NOT NULL,
    v_version_major          INTEGER               NOT NULL,
    v_version_minor          INTEGER               NOT NULL,
    v_version_patch          INTEGER               NOT NULL,
    v_version_pre_release_no INTEGER,
    v_is_pre_release_version BOOLEAN default false NOT NULL,
    v_created_timestamp      TIMESTAMP             NOT NULL
);
ALTER TABLE updateservice.version OWNER TO updateservice;
CREATE UNIQUE INDEX "version_string_idx" ON updateservice.version (v_version_string);

CREATE SEQUENCE updateservice.version_file_id_seq START WITH 1 INCREMENT BY 1;
ALTER SEQUENCE updateservice.version_file_id_seq OWNER TO updateservice;
CREATE TABLE updateservice.version_file
(
    f_id                  INTEGER DEFAULT nextval('updateservice.version_id_seq') PRIMARY KEY,
    f_file_provider       INTEGER   NOT NULL,
    f_platform_identifier INTEGER   NOT NULL,
    f_file_name           TEXT      NOT NULL,
    f_hash                TEXT      NOT NULL,
    f_file_size           BIGINT    NOT NULL,
    f_version_ref         INTEGER   NOT NULL CONSTRAINT "version_file_version_ref_fk" REFERENCES updateservice.version ON DELETE CASCADE,
    f_created_timestamp   TIMESTAMP NOT NULL
);
ALTER TABLE updateservice.version_file OWNER TO updateservice;
CREATE INDEX "version_file_version_ref_idx" ON updateservice.version_file (f_version_ref);