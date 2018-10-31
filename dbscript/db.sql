drop table if exists oa_project;
create table oa_project(
    id                  int             not null auto_increment comment 'primary key',
    name                varchar(100)    not null comment 'project name',
    code                varchar(30)     not null comment 'project code',
    contact             varchar(50)     not null comment 'project contact from customer',
    start_date          datetime        not null comment 'project start date',
    stop_date           datetime        not null comment 'project stop date',
    state               int             not null comment 'project state,1-going,2-stopped',
    primary key(id)
)engine=innodb default charset=utf8 comment 'project information';

drop table if exists oa_employee;
create table oa_employee(
    id                  int             not null auto_increment comment 'primary key',
    name                varchar(50)     not null comment 'employee name',
    job                 int             not null comment 'employee position',
    ding_id             varchar(50)     not null comment 'dingding id',
    primary key(id)
)engine=innodb default charset=utf8 comment 'employee to finish project';

drop table if exists oa_employee_ding;
create table oa_employee_ding(
    id                  int             not null auto_increment comment 'primary key',
    emp_id              int             not null comment 'employee id',
    ding_id             varchar(50)     not null comment 'employee id at dingding system',
    primary key(id)
)engine=innodb default charset=utf8 comment 'employee assoation with dingding';


drop table if exists oa_project_staff;
create table oa_project_staff(
    id                  int             not null auto_increment comment 'primary key',
    proj_id             int             not null comment 'project id',
    emp_id              int             not null comment 'employee id',
    start_date          datetime        not null comment 'enroll start date',
    stop_date           datetime        not null comment 'leave date',
    period              int             not null comment 'project month,201810',
    primary key(id),
    index(proj_id),
    index(emp_id)
)engine=innodb default charset=utf8 comment 'employee enroll project information';

drop table if exists oa_check_time;
create table oa_check_time(
    id                  bigint          not null auto_increment comment 'primary key',
    ding_id             varchar(50)     not null comment 'employee id',
    check_in            datetime        not null comment 'check in time',
    check_out           datetime        not null comment 'check out time',
    check_out_real      datetime        not null comment 'real check out time',
    check_date          datetime        not null comment 'check date',
    primary key(id),
    index(ding_id)
)engine=innodb default charset=utf8 comment 'employee daily check time';

drop table if exists oa_user;
create table oa_user(
    id                  int             not null auto_increment comment 'user_id primary key',
    name                varchar(50)     not null comment 'user name',
    pwd                 varchar(128)    not null comment 'user pwd',
    primary key(id)
)engine=innodb default charset=utf8 comment 'user information';


