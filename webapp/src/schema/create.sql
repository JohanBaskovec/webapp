-- blog database definition


create table article (
  id serial primary key,
  title text default '',
  content text default ''
);
