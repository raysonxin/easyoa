package models

import (
	"time"
)

// CheckTimeModel check time model
type CheckTimeModel struct {
	Id           int       `gorm:"column:id;primary_key"` //
	DingId       string    `gorm:"column:ding_id"`        //
	CheckIn      time.Time `gorm:"column:check_in"`       //
	CheckOut     time.Time `gorm:"column:check_out"`
	CheckOutReal time.Time `gorm:"column:check_out_real"`
	CheckDate    time.Time `gorm:"column:check_date"`
	EmpId        int       `gorm:"column:emp_id"`
}

// TableName return tablename
func (c CheckTimeModel) TableName() string {
	return "oa_check_time"
}
