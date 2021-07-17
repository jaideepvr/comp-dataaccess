-- Create user table
CREATE TABLE "user" (
  "user_id" integer NOT NULL,
  "user_name" text NOT NULL,
  "password" text NOT NULL,
  "first_name" text,
  "last_name" text,
  "date_of_birth" text NOT NULL,
  PRIMARY KEY ("user_id" AUTOINCREMENT)
);

INSERT
  INTO "user"
       (user_name, password, first_name, last_name, date_of_birth)
VALUES ('jaideep', 'a', 'Jaideep', 'Vinakota', '1972-06-08');
INSERT
  INTO "user"
       (user_name, password, first_name, last_name, date_of_birth)
VALUES ('sridevi', 'a', 'Sridevi', 'Vinakota', '1973-01-10');
