var domain = "http://18.217.201.183/v1";
//var domain = "https://localhost:44344/v1";

export default {
  loginUser: `${domain}/auth/login`,
  logoutUser: `${domain}/auth/logout/userId`,
  loadDashboard: `${domain}/dashboard/allwidget`,
  postDashboard: `${domain}/dashboard/addorupdatewidget`,
  deleteDashboard: `${domain}/dashboard/deletewidget`,
  editWidget: `${domain}/dashboard/editwidget`,
  drilldownDashboard: `${domain}/dashboard/dashboardwidgetclick`,
  getuserDetails: `${domain}/auth/viewallusers`,
  postuserDetails: `${domain}/auth/userconfig`,
  getReportName: `${domain}/reports/reportnames`,
  viewReportName: `${domain}/reports/viewreport`,
  editReportName: `${domain}/reports/editreport`,
  postReportName: `${domain}/reports/addeditreport`,
  downloadReportName: `${domain}/reports/downloadreport`,
  gridEmailSend: `${domain}/dashboard/gridsendemailsms`,
  getSchedulerNames:`${domain}/reports/schedulernames`,
  editSchedulerNames:`${domain}/reports/editscheduler`,
  addUpdateSchedular:`${domain}/reports/addupdatescheduler`,
  allWidgetDropDowns:`${domain}/dashboard/allwidgetdropDowns`,

  //loadDashboard:`${domain}/dashboard`,
};
