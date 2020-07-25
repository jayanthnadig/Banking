//var domain ="http://localhost:4000/api";
var domain = "https://localhost:44344/v1";

export default {
  loginUser: `${domain}/auth/login`,
  logoutUser: `${domain}/auth/logout/userId`,
  loadDashboard: `${domain}/dashboard/allwidget`,
  postDashboard: `${domain}/dashboard/addoreditwidget`,
  deleteDashboard: `${domain}/dashboard/deletewidget`,
  drilldownDashboard: `${domain}/dashboard/dashboardwidgetclick`,
  getuserDetails: `${domain}/auth/viewallusers`,
  postuserDetails: `${domain}/auth/userconfig`,
  getReportName: `${domain}/reports/reportnames`,
  viewReportName: `${domain}/reports/viewreport`,
  editReportName: `${domain}/reports/editreport`,
  postReportName: `${domain}/reports/addeditreport`,
  downloadReportName: `${domain}/reports/downloadreport`,
  gridEmailSend: `${domain}/dashboard/gridsendemail`,
  //loadDashboard:`${domain}/dashboard`,
};
