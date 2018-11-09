package models

import (
	"time"
)

type EmployeeWorkloadModel struct {
	DingId    string `gorm:"column:ding_id"`
	EmpName   string `gorm:"column:emp_name"`
	EmpJob    string
	DayCount  int     `gorm:"column:day_count"`
	HourCount float64 `gorm:"column:hour_count"`
	Total     float64 `gorm:"column:total"`

	CheckRecords []CheckTimeModel `gorm:"-"`
}

type ProjectWorkloadModel struct {
	ProjId      int
	ProjName    string
	ProjCode    string
	Contact     string
	ProjStart   time.Time
	ProjStop    time.Time
	Workload    float64
	EmpWorkload []EmployeeWorkloadModel
}
