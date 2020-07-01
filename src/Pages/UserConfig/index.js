import React from "react";
import * as am4core from "@amcharts/amcharts4/core";
import * as am4charts from "@amcharts/amcharts4/charts";
import logo from "../../dist/images/logo.png";

class UserConfig extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      modal: false,
    };
  }
  componentDidMount() {}

  componentWillUnmount() {}
  showAddWidget(flag) {
    this.setState((prevState) => ({
      // prevState?
      modal: flag,
    }));
  }
  logout() {
    window.location.href = "/";
  }
  render() {
    return (
      <>
        <div className="app">
          <div className="border-b border-theme-24 -mt-10 md:-mt-5 -mx-3 sm:-mx-8 px-3 sm:px-8 pt-3 md:pt-0 mb-10">
            <div className="top-bar-boxed flex items-center">
              <a href="" className="-intro-x hidden md:flex">
                <img
                  alt="Midone Tailwind HTML Admin Template"
                  className="w-6"
                  src={logo}
                />
                <span className="text-white text-lg ml-3"> </span>
              </a>

              {/* <div className="-intro-x breadcrumb breadcrumb--light mr-auto">
                {" "}
                <a href="" className="">
                  Application
                </a>{" "}
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width="24"
                  height="24"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  stroke-width="1.5"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  class="feather feather-chevron-right breadcrumb__icon"
                >
                  <polyline points="9 18 15 12 9 6"></polyline>
                </svg>{" "}
                <a href="" className="breadcrumb--active">
                  Dashboard
                </a>{" "}
              </div> */}
              <div
                class="intro-x dropdown relative mr-4 sm:mr-6"
                style={{ width: "100%" }}
              >
                <span className="custom_logout">Logout</span>
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width="24"
                  height="24"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="#000"
                  stroke-width="1.5"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  className="feather feather-log-out mx-auto custom_logout"
                  onClick={() => this.logout()}
                >
                  <path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"></path>
                  <polyline points="16 17 21 12 16 7"></polyline>
                  <line x1="21" y1="12" x2="9" y2="12"></line>
                </svg>
                <span className="userwelcome">Welcome Santhosh</span>
              </div>
            </div>
          </div>
          <div class="flex">
            <nav class="side-nav">
              <ul>
                <li>
                  <a href="/dashboard" class="side-menu ">
                    <div class="side-menu__icon">
                      {" "}
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        width="24"
                        height="24"
                        viewBox="0 0 24 24"
                        fill="none"
                        stroke="currentColor"
                        stroke-width="1.5"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                        class="feather feather-home"
                      >
                        <path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"></path>
                        <polyline points="9 22 9 12 15 12 15 22"></polyline>
                      </svg>{" "}
                    </div>
                    <div class="side-menu__title"> Dashboard </div>
                  </a>
                </li>

                <li>
                  <a href="/reportconfig" class="side-menu ">
                    <div class="side-menu__icon">
                      {" "}
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        width="24"
                        height="24"
                        viewBox="0 0 24 24"
                        fill="none"
                        stroke="currentColor"
                        stroke-width="1.5"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                        class="feather feather-inbox"
                      >
                        <polyline points="22 12 16 12 14 15 10 15 8 12 2 12"></polyline>
                        <path d="M5.45 5.11L2 12v6a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2v-6l-3.45-6.89A2 2 0 0 0 16.76 4H7.24a2 2 0 0 0-1.79 1.11z"></path>
                      </svg>{" "}
                    </div>
                    <div class="side-menu__title">
                      {" "}
                      Reports{" "}
                      <i
                        data-feather="chevron-down"
                        class="menu__sub-icon"
                      ></i>{" "}
                    </div>
                  </a>
                </li>

                <li>
                  <a href="#" class="side-menu side-menu--active">
                    <div class="side-menu__icon">
                      {" "}
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        width="24"
                        height="24"
                        viewBox="0 0 24 24"
                        fill="none"
                        stroke="currentColor"
                        stroke-width="1.5"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                        class="feather feather-mail mx-auto"
                      >
                        <path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z"></path>
                        <polyline points="22,6 12,13 2,6"></polyline>
                      </svg>{" "}
                    </div>
                    <div class="side-menu__title"> User Configuration </div>
                  </a>
                </li>
              </ul>
            </nav>
           
            <div className="content report_content">
              <div class="intro-y flex items-center mt-8">
                <h2 class="text-lg font-medium mr-auto"> </h2>
              </div>
              <div
                className={`intro-y grid grid-cols-12 ${
                  this.state.modal ? "hidden" : ""
                }`}
              >
                <div class="col-span-12 lg:col-span-12">
                  <div class="intro-y box">
                    <div class="flex flex-col sm:flex-row items-center p-5 border border-gray-200">
                      <h1 class="font-medium text-base mr-auto text-4xl ">
                       User Configuration
                      </h1>
                    </div>
                    <div class="p-5" id="vertical-bar-chart">
                    <div class="modal__content relative custom_model_content">
                            <div class="flex flex-col sm:flex-row items-center">
                              {" "}
                              <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                User Name
                              </label>{" "}
                              <input
                                type="text"
                                class="input w-full border mt-2 flex-1"
                                placeholder="User Name"
                              />{" "}
                            </div>{" "}
                            <div class="flex flex-col sm:flex-row items-center">
                              {" "}
                              <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                Password
                              </label>{" "}
                              <input
                                type="password"
                                class="input w-full border mt-2 flex-1"
                                placeholder=""
                              />{" "}
                            </div>{" "}
                         
                            <div class="flex flex-col sm:flex-row items-center">
                              {" "}
                              <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                Email
                              </label>{" "}
                              <input
                                type="text"
                                class="input w-full border mt-2 flex-1"
                                placeholder="User Email-Id"
                              />{" "}
                            </div>{" "}
                           
                            
                            <div class="sm:ml-20 sm:pl-5 mt-5">
                              {" "}
                              <button
                                type="button"
                                class="button bg-theme-1 text-white"
                                
                              >
                                Create
                              </button>{" "}
                              <button
                                type="button"
                                class="button bg-theme-1 text-white"
                               
                              >
                                Cancel
                              </button>{" "}
                            </div>
                          </div>{" "}
                      <div class="preview">
                        <div class="overflow-x-auto">
                          <h3
                            class="font-medium text-base mr-auto text-2xl "
                            style={{ "margin-bottom": "30px" }}
                          >
                            Detailed View of Unauthorised Transactions
                          </h3>{" "}
                          <table class="table">
                            {" "}
                            <thead>
                              {" "}
                              <tr class="text-white">
                                {" "}
                                <th class="border border-b-2 whitespace-no-wrap">
                                SLNO
                                </th>{" "}
                                <th class="border border-b-2 whitespace-no-wrap">
                               User Name
                                </th>{" "}
                                <th class="border border-b-2 whitespace-no-wrap">
                                Password
                                </th>{" "}
                                <th class="border border-b-2 whitespace-no-wrap">
                                Email
                                </th>{" "}
                                <th class="border border-b-2 whitespace-no-wrap">
                                IsActive
                                </th>{" "}
                              </tr>{" "}
                            </thead>{" "}
                            <tbody>
                              {" "}
                              <tr>
                                {" "}
                                <td class="border">1</td>{" "}
                                <td class="border">Ajay</td>{" "}
                                <td class="border">********</td>{" "}
                                <td class="border">
                                  ajay@gmail.com
                                </td>{" "}
                                <td class="border">
                                <input type="checkbox" class="input border mr-2" id="vertical-remember-me"/>
                                </td>{" "}
                              </tr>{" "}
                              <tr>
                                {" "}
                                <td class="border">2</td>{" "}
                                <td class="border">Arjun</td>{" "}
                                <td class="border">********</td>{" "}
                                <td class="border">
                                  arjun@gmail.com
                                </td>{" "}
                                <td class="border">
                                <input type="checkbox" class="input border mr-2" id="vertical-remember-me" checked/>
                                </td>{" "}
                              </tr>{" "}
                              <tr>
                                {" "}
                                <td class="border">3</td>{" "}
                                <td class="border">Ashok</td>{" "}
                                <td class="border">********</td>{" "}
                                <td class="border">
                                  ashok@gmail.com
                                </td>{" "}
                                <td class="border">
                                <input type="checkbox" class="input border mr-2" id="vertical-remember-me"/>
                                </td>{" "}
                              </tr>{" "}
                              <tr>
                                {" "}
                                <td class="border">4</td>{" "}
                                <td class="border">Vijay</td>{" "}
                                <td class="border">********</td>{" "}
                                <td class="border">
                                  vijay@gmail.com
                                </td>{" "}
                                <td class="border">
                                <input type="checkbox" class="input border mr-2" id="vertical-remember-me"/>
                                </td>{" "}
                              </tr>{" "}
                              <tr>
                                {" "}
                                <td class="border">5</td>{" "}
                                <td class="border">Trisha</td>{" "}
                                <td class="border">********</td>{" "}
                                <td class="border">
                                  trisha@gmail.com
                                </td>{" "}
                                <td class="border">
                                <input type="checkbox" class="input border mr-2" id="vertical-remember-me"/>
                                </td>{" "}
                              </tr>{" "}
                            </tbody>{" "}
                          </table>{" "}
                          <div class="sm:ml-20 sm:pl-5 mt-5">
                              {" "}
                              <button
                                type="button"
                                class="button bg-theme-1 text-white"
                                style={{"float":"right"}}
                              >
                                Update
                              </button>{" "}
                              
                            </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              <div
                className={`intro-y grid grid-cols-12 ${
                  !this.state.modal ? "hidden" : ""
                }`}
              >
                <div class="col-span-12 lg:col-span-12">
                  <div class="intro-y box">
                    <div class="flex flex-col sm:flex-row items-center p-5 border border-gray-200">
                      <h1 class="font-medium text-base mr-auto text-4xl ">
                        Config Reports
                      </h1>
                    </div>
                    <div class="p-5" id="vertical-bar-chart">
                      <div class="preview">
                        <div class="overflow-x-auto">
                          {" "}
                          <div class="modal__content relative custom_model_content">
                            <div class="flex flex-col sm:flex-row items-center">
                              {" "}
                              <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                Name
                              </label>{" "}
                              <input
                                type="text"
                                class="input w-full border mt-2 flex-1"
                                placeholder="Store Performance"
                              />{" "}
                            </div>{" "}
                            <div class="flex flex-col sm:flex-row items-center">
                              {" "}
                              <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                Query
                              </label>{" "}
                              <textarea
                                class="input w-full border mt-2 flex-1"
                                placeholder="Select statement"
                                style={{ height: "400px" }}
                              />{" "}
                            </div>{" "}
                            <div class="flex flex-col sm:flex-row items-center">
                              {" "}
                              <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                Email
                              </label>{" "}
                              <input
                                type="text"
                                class="input w-full border mt-2 flex-1"
                                placeholder="Store Performance"
                              />{" "}
                            </div>{" "}
                            <div class="flex flex-col sm:flex-row items-center mt-3">
                              {" "}
                              <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                File Format
                              </label>{" "}
                              <select class="select2 w-full input w-full border mt-2 flex-1">
                                <option value="1">Excel</option>
                                <option value="2">PDF</option>
                              </select>{" "}
                            </div>{" "}
                            <div class="flex flex-col sm:flex-row items-center mt-3">
                              {" "}
                              <label class="w-full sm:w-20 sm:text-right sm:mr-5">
                                Send Report Every
                              </label>{" "}
                              <select class="select2 w-full input w-full border mt-2 flex-1">
                                <option value="1">1 Hour</option>
                                <option value="2">4 Hours</option>
                                <option value="3">6 Hours</option>
                                <option value="3">Once per day</option>
                              </select>{" "}
                            </div>{" "}
                            <div class="sm:ml-20 sm:pl-5 mt-5">
                              {" "}
                              <button
                                type="button"
                                class="button bg-theme-1 text-white"
                                onClick={() => this.showAddWidget(false)}
                              >
                                Submit
                              </button>{" "}
                              <button
                                type="button"
                                class="button bg-theme-1 text-white"
                                onClick={() => this.showAddWidget(false)}
                              >
                                Cancel
                              </button>{" "}
                            </div>
                          </div>{" "}
                        </div>
                      </div>
                    
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </>
    );
  }
}
export default UserConfig;
