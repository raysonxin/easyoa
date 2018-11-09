package models

// CheckTimeModel check time model
type CheckTimeModel struct {
	//	Id           int       `gorm:"column:id;primary_key"` //
	DingId   string `gorm:"column:ding_id"`  //
	CheckIn  string `gorm:"column:check_in"` //
	CheckOut string `gorm:"column:check_out"`
	//CheckOutReal string    `gorm:"column:check_out_real"`
	CheckDate int `gorm:"column:check_date"`
	//EmpId        int       `gorm:"column:emp_id"`

	OvertimeHour float64 `gorm:"-"`
}

// TableName return tablename
func (c CheckTimeModel) TableName() string {
	return "oa_check_time"
}
