package v1

import (
	"fmt"
	"net/http"
	"strconv"
	"time"

	"github.com/jinzhu/gorm"
	"github.com/raysonxin/easyoa/oaserver/models"
	"github.com/raysonxin/easyoa/oaserver/toolkits/httpctx"
)

type ReportController struct {
	Orm *gorm.DB
}

func (c *ReportController) GetProjectWorkPeriod(ctx *httpctx.HTTPCtx, db *gorm.DB) {

	start := ctx.Query("start")
	if start == "" {
		ctx.Error(http.StatusBadRequest, "request start proj lost")
		return
	}

	stop := ctx.Query("stop")
	if stop == "" {
		ctx.Error(http.StatusBadRequest, "request stop proj lost")
		return
	}

	projId := ctx.QueryInt("proj")
	if projId == 0 {
		ctx.Error(http.StatusBadRequest, "request proj proj lost")
		return
	}

	records, err := c.getEmployeePeriodForProject(projId, start, stop)
	if err != nil {
		ctx.Error(http.StatusBadRequest, err.Error())
		return
	}

	ctx.Success(http.StatusOK, records)
}

func (c *ReportController) GetProjectWorkload(ctx *httpctx.HTTPCtx, db *gorm.DB) {
	start := ctx.Query("start")
	if start == "" {
		ctx.Error(http.StatusBadRequest, "request start proj lost")
		return
	}

	stop := ctx.Query("stop")
	if stop == "" {
		ctx.Error(http.StatusBadRequest, "request stop proj lost")
		return
	}

	projId := ctx.QueryInt("proj")
	if projId == 0 {
		ctx.Error(http.StatusBadRequest, "request proj proj lost")
		return
	}

	periods, err := c.getEmployeePeriodForProject(projId, start, stop)
	if err != nil {
		ctx.Error(http.StatusBadRequest, err.Error())
		return
	}

	result := make([]models.EmployeeWorkloadModel, 0)

	for _, val := range periods {
		workload, err := c.getEmployeeWorkload(val)
		if err != nil {
			one := models.EmployeeWorkloadModel{
				DingId:    val.DingId,
				DayCount:  0,
				HourCount: 0,
				Total:     0,
			}
			result = append(result, one)
			continue
		}

		result = append(result, workload)
	}

	ctx.Success(http.StatusOK, result)
}

func (c *ReportController) GetMonthly(ctx *httpctx.HTTPCtx, db *gorm.DB) {
	year := ctx.QueryInt("year")
	if year == 0 {
		ctx.Error(http.StatusBadRequest, "request year proj lost")
		return
	}

	month := ctx.QueryInt("month")
	if month == 0 {
		ctx.Error(http.StatusBadRequest, "request month proj lost")
		return
	}

	startStr := fmt.Sprintf("%d-%d-01", year, month)
	nextStr := fmt.Sprintf("%d-%d-01", year, month+1)
	nextMonth, _ := time.Parse("2006-01-02", nextStr)
	stopTime := nextMonth.AddDate(0, 0, -1)
	stopStr := stopTime.Format("2006-01-02")

	//query project
	var projects []models.ProjectModel
	err := db.Table("oa_project").Scan(&projects).Error
	if err != nil {
		ctx.Error(http.StatusBadRequest, err.Error())
		return
	}

	projStats := make([]models.ProjectWorkloadModel, 0)
	for _, proj := range projects {
		empWorkloads, err := c.getProjectWorkload(proj.Id, startStr, stopStr)
		if err != nil {
			continue
		}

		var counter float64 = 0.0
		for _, emp := range empWorkloads {
			counter = counter + emp.Total
		}

		pwd := models.ProjectWorkloadModel{
			ProjId:      proj.Id,
			ProjName:    proj.Name,
			ProjCode:    proj.Code,
			ProjStart:   proj.StartDate,
			ProjStop:    proj.StopDate,
			EmpWorkload: empWorkloads,
			Workload:    counter,
		}

		projStats = append(projStats, pwd)
	}

	ctx.Success(http.StatusOK, projStats)
}

func (c *ReportController) getEmployeePeriodForProject(projId int, start, stop string) ([]models.ProjectStaffRecordModel, error) {

	sql := fmt.Sprintf(`select * from oa_project_staff_record where 
	((start_date<='%s' and stop_date>='%s') or 
	(start_date>='%s' and stop_date<='%s') or 
	(start_date>='%s' and start_date<='%s')) and proj_id=%d`, start, start, start, stop, start, stop, projId)

	var records []models.ProjectStaffRecordModel
	err := c.Orm.Raw(sql).Scan(&records).Error
	if err != nil {
		return nil, err
	}

	startDate, _ := time.Parse("2006-01-02", start)
	stopDate, _ := time.Parse("2006-01-02", stop)
	for key, val := range records {
		if val.StartDate.Before(startDate) {
			records[key].StartDate = startDate
		}

		if stopDate.Before(val.StopDate) {
			records[key].StopDate = stopDate
		}
	}
	return records, nil
}

func (c *ReportController) getChecktimeByPeriods(period models.ProjectStaffRecordModel) ([]models.CheckTimeModel, error) {

	start, _ := strconv.Atoi(period.StartDate.Format("20060102"))
	stop, _ := strconv.Atoi(period.StopDate.Format("20060102"))

	var records []models.CheckTimeModel

	err := c.Orm.Table("oa_check_time").Where("ding_id=? and check_date>=? and check_date<=?", period.DingId, start, stop).Scan(&records).Error
	if err != nil {
		return nil, err
	}
	return records, nil
}

func (c *ReportController) getEmployeeWorkload(period models.ProjectStaffRecordModel) (models.EmployeeWorkloadModel, error) {
	checkRecords, err := c.getChecktimeByPeriods(period)
	if err != nil {
		return models.EmployeeWorkloadModel{}, err
	}

	dayCounter := 0
	var hourCounter float64 = 0.0
	for index, val := range checkRecords {
		if val.CheckIn == "" && val.CheckOut == "" {
			continue
		}
		dayCounter++

		if val.CheckOut == "" || val.CheckOut < "17:30" {
			checkRecords[index].CheckOut = "17:30"
		}

		offTimeStr := fmt.Sprintf("%d %s:00", val.CheckDate, checkRecords[index].CheckOut)
		offTime, _ := time.Parse("20060102 15:04:05", offTimeStr)

		ruleTimeStr := fmt.Sprintf("%d 17:30:00", val.CheckDate)
		ruleTime, _ := time.Parse("20060102 15:04:05", ruleTimeStr)

		duration := offTime.Sub(ruleTime)
		hourCounter = hourCounter + duration.Hours()
		checkRecords[index].OvertimeHour = duration.Hours()
	}

	var emp models.EmployeeModel
	err = c.Orm.Table("oa_employee").Find(&emp, "ding_id=?", period.DingId).Error
	if err != nil {
		return models.EmployeeWorkloadModel{}, err
	}

	result := models.EmployeeWorkloadModel{
		DingId:       period.DingId,
		EmpName:      emp.Name,
		EmpJob:       emp.Job,
		DayCount:     dayCounter,
		HourCount:    hourCounter,
		Total:        float64(dayCounter) + (hourCounter / 8),
		CheckRecords: checkRecords,
	}

	return result, nil
}

func (c *ReportController) getProjectWorkload(projId int, start, stop string) ([]models.EmployeeWorkloadModel, error) {

	periods, err := c.getEmployeePeriodForProject(projId, start, stop)
	if err != nil {
		return nil, err
	}

	result := make([]models.EmployeeWorkloadModel, 0)

	for _, val := range periods {
		workload, err := c.getEmployeeWorkload(val)
		if err != nil {
			one := models.EmployeeWorkloadModel{
				DingId:    val.DingId,
				DayCount:  0,
				HourCount: 0,
				Total:     0,
			}
			result = append(result, one)
			continue
		}

		result = append(result, workload)
	}

	return result, nil
}
