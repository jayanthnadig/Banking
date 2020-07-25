import * as qs from "query-string";
import lookupUtility from "../Common/Utility/LookUpDataMapping";
let _userDetails=lookupUtility.LoginDetails();
function filterJSON(res) {
  console.log("response");
  return res.json();
}
function filterStatus(res) {
  if (res.status >= 200 && res.status < 300) {
    return res;
  }
  const error = new Error(res.statusText);
  error.res = res;
  error.type = "http";
  throw error;
}
export function get(urlString, params) {
  let url = urlString;
  if (params) {
    url = `${urlString}/${params}`;
  }
  return fetch(url, {
    headers: {
      //"Cookie": key
      "Cache-control": " no-cache",
      "Cache-control": "no-store",
      Pragma: "no-cache",
      Expires: "0",
     'Access-Control-Allow-Origin':'*',
     'x-client-token':_userDetails.token
    },
    //credentials: "include",
  })
    .then(filterStatus)
    .then(filterJSON);
}

export function put(url, body) {
  return fetch(url, {
    method: "PUT",
    headers: { Accept: "application/json", "Content-Type": "application/json", 'x-client-token':_userDetails.token },
    credentials: "include",
    body: JSON.stringify(body),
  })
    .then(filterStatus)
    .then(filterJSON);
}
export function post(url, body) {
  // let myHeaders = new Headers();  // myHeaders.append('Content-Type','application/json');

  return fetch(url, {
    method: "POST",
    headers: { Accept: "application/json", "Content-Type": "application/json",'Access-Control-Allow-Origin':'*', 'x-client-token':(_userDetails)?_userDetails.token:"" },
    //credentials: "include",
    body: (body)?JSON.stringify(body):"",
  })
    .then(filterStatus)
    .then(filterJSON);
}

export function postquery(urlString,body,params) {
  let url = urlString;
  if (params) {
    url = `${urlString}/${params}`;
  }
  return fetch(url, {
    method: "POST",
    headers: {
      //"Cookie": key
      "Cache-control": " no-cache",
      "Cache-control": "no-store",
      Pragma: "no-cache",
      Expires: "0",
     'Access-Control-Allow-Origin':'*',
     'Content-Type':'application/json',
     'x-client-token':_userDetails.token
    },
    body: JSON.stringify(body),
    //credentials: "include",
  })
    .then(filterStatus)
    .then(filterJSON);
}

export function deleteQuery(urlString,params,_id) {
  let url = urlString;
  if (params) {
    url = `${urlString}/${params}/widgetId/${_id}`;
  }
  return fetch(url, {
    method: "DELETE",
    headers: {
      //"Cookie": key
      "Cache-control": " no-cache",
      "Cache-control": "no-store",
      Pragma: "no-cache",
      Expires: "0",
     'Access-Control-Allow-Origin':'*',
     'Content-Type':'application/json',
     'x-client-token':_userDetails.token
    },
   // body: JSON.stringify(body),
    //credentials: "include",
  })
    .then(filterStatus)
    .then(filterJSON);
}

export function getQuery(urlString,params,_id) {
  let url = urlString;
  if (params) {
    url = `${urlString}/${params}/reportId/${_id}`;
  }
  return fetch(url, {
    method: "GET",
    headers: {
      //"Cookie": key
      "Cache-control": " no-cache",
      "Cache-control": "no-store",
      Pragma: "no-cache",
      Expires: "0",
     'Access-Control-Allow-Origin':'*',
     'Content-Type':'application/json',
     'x-client-token':_userDetails.token
    },
   // body: JSON.stringify(body),
    //credentials: "include",
  })
    .then(filterStatus)
    .then(filterJSON);
}


export function getFiles(urlString,params,_id) {
  let url = urlString;
  if (params) {
    url = `${urlString}/${params}/reportId/${_id}`;
  }
  return fetch(url, {
    method: "GET",
    headers: {
      //"Cookie": key
      "Accept": "text/csv",
      "Cache-control": " no-cache",
      "Cache-control": "no-store",
      Pragma: "no-cache",
      Expires: "0",
     'Access-Control-Allow-Origin':'*',
     'Content-Type':'text/csv ',
     'x-client-token':_userDetails.token
    },
   // body: JSON.stringify(body),
    //credentials: "include",
  })
  .then((response) => response.text())
  .then((responseText) => responseText)
  .catch((error) => {
     // this.setState({downloadingCSV: false})
      console.error("CSV handleDownloadClick:", error)
  })
}