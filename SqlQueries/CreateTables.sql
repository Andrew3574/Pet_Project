--create extension if not exists "uuid-ossp" schema "public";
--select uuid_generate_v4();

create table Roles(
	role_id serial primary key,
	name varchar(256) not null
);

create table Categories(
	category_id serial primary key,
	name varchar(256) not null
);

create table Events(
	event_id uuid primary key default uuid_generate_v4(),
	name varchar(256) not null,
	description varchar(256),
	event_date timestamp not null default current_timestamp,
	location varchar(256) not null,
	category_id int references categories(category_id) on delete cascade,
	guest_limit int not null check (guest_limit >= 0),
	image varchar(256)
);

create table Guests(
	guest_id uuid primary key default uuid_generate_v4(),
	role_id int references Roles(role_id) on delete restrict,
	name varchar(256) not null,
	surname varchar(256) not null,
	birth_date date not null,
	email varchar(256) unique not null
);

create table Shared_Events_Guests(
	id serial primary key,
	event_id uuid references Events(event_id) on delete cascade,
	guest_id uuid references Guests(guest_id) on delete cascade,	
	registration_date timestamp default current_timestamp
);

delete from guests;
delete from events;
delete from Shared_Events_Guests;

drop table Shared_Events_Guests;
drop table guests;
drop table events;