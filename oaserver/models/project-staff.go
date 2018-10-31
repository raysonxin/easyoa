package models

import (
	"time"
)

// ProjectStaffModel employee enroll project
type ProjectStaffModel struct {
	Id        int       `gorm:"column:id;primary_key"` //id
	ProjId    int       `gorm:"column:proj_id"`        //project id
	EmpId     int       `gorm:"column:emp_id"`         //employee id
	StartDate time.Time `gorm:"column:start_date"`     //start date
	StopDate  time.Time `gorm:"column:stop_date"`      //stop date
	Period    int       `gorm:"column:period"`         //period,year and month
}

// TableName get table name
func (m ProjectStaffModel) TableName() string {
	return "oa_project_staff"
}
