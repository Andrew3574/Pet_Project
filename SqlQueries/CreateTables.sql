--create extension if not exists "uuid-ossp" schema "public";
--select uuid_generate_v4();

create table Categories(
	category_id serial primary key,
	name varchar(256) not null
);

create table Events(
	event_id uuid primary key default uuid_generate_v4(),
	name varchar(256) not null,
	description varchar(256),
	event_date timestamptz not null default current_timestamp,
	location varchar(256) not null,
	category_id int references categories(category_id) on delete cascade,
	guest_limit int not null check (guest_limit >= 0),
	image varchar(256)
);

create table Guests(
	guest_id uuid primary key default uuid_generate_v4(),
	name varchar(256) not null,
	surname varchar(256) not null,
	birth_date date not null,
	registration_date timestamptz default current_timestamp,
	email varchar(256) not null
);

create table Shared_Events_Guests(
	id serial primary key,
	event_id uuid references Events(event_id) on delete cascade,
	guest_id uuid references Guests(guest_id) on delete cascade
);

--delete from guests
--delete from events

--drop table guests
--drop table events
--drop table Shared_Events_Guests