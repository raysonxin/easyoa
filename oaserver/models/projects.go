package models

import (
	"time"
)

// ProjectModel project model
type ProjectModel struct {
	Id        int       `gorm:"column:id;primary_key"` //
	Name      string    `gorm:"column:name"`           //
	Code      string    `gorm:"column:code"`           //
	Contact   string    `gorm:"column:contact"`        //
	StartDate time.Time `gorm:"column:start_date"`     //
	StopDate  time.Time `gorm:"column:stop_date"`      //
	State     int       `gorm:"column:state"`          //
}

// TableName 表名称
func (m ProjectModel) TableName() string {
	return "oa_project"
}
