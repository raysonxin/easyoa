package models

import (
	"time"
)

// ProjectStaffModel employee enroll project
type ProjectStaffModel struct {
	Id        int       `gorm:"column:id;primary_key"` //id
	ProjId    int       `gorm:"column:proj_id"`        //project id
	DingId    string    `gorm:"column:ding_id"`        //employee id
	StartDate time.Time `gorm:"column:start_date"`     //start date
	Name      string    `gorm:"-"`
	Job       string    `gorm:"-"`
}

func (m ProjectStaffModel) TableName() string {
	return "oa_project_staff"
}

// ProjectStaffModel employee enroll project
type ProjectStaffRecordModel struct {
	Id        int       `gorm:"column:id;primary_key"` //id
	ProjId    int       `gorm:"column:proj_id"`        //project id
	DingId    string    `gorm:"column:ding_id"`        //employee id
	StartDate time.Time `gorm:"column:start_date"`     //start date
	StopDate  time.Time `gorm:"column:stop_date"`      //stop date
}

// TableName get table name
func (m ProjectStaffRecordModel) TableName() string {
	return "oa_project_staff_record"
}
