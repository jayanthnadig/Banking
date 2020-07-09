import * as qs from "query-string";
function filterJSON(res) {
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
    url = `${urlString}/${qs.stringify(params)}`;
  }
  return fetch(url, {
    headers: {
      //"Cookie": key
      "Cache-control": " no-cache",
      "Cache-control": "no-store",
      Pragma: "no-cache",
      Expires: "0",
     'Access-Control-Allow-Origin':'*'
    },
    //credentials: "include",
  })
    .then(filterStatus)
    .then(filterJSON);
}

export function put(url, body) {
  return fetch(url, {
    method: "PUT",
    headers: { Accept: "application/json", "Content-Type": "application/json" },
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
    headers: { Accept: "application/json", "Content-Type": "application/json",'Access-Control-Allow-Origin':'*' },
    //credentials: "include",
    body: JSON.stringify(body),
  })
    .then(filterStatus)
    .then(filterJSON);
}

export function postquery(urlString,body,params) {
  let url = urlString;
  if (params) {
    url = `${urlString}/${qs.stringify(params)}`;
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
    url = `${urlString}/${qs.stringify(params)}/widgetId/${_id}`;
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
    url = `${urlString}/${qs.stringify(params)}/reportId${_id}`;
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
    },
   // body: JSON.stringify(body),
    //credentials: "include",
  })
    .then(filterStatus)
    .then(filterJSON);
}