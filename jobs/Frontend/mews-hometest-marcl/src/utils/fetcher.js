const activeRequests = {};


export const fetcher = ({ key, baseURL, endpointTag, method, headers, ...rest }) => {
 let accurateHeaders = {
   ...headers,
   Accept: "text/plain",
   "Content-Type": "application/json",
 };


 // If there's an active request for the same endpoint, cancel it
 if (key !== undefined && activeRequests[key]) {
   activeRequests[key].cancel();
 }


 const controller = new AbortController();
 const signal = controller.signal;

 const fullURL = baseURL + endpointTag;

 const request = fetch(fullURL, {
   headers: accurateHeaders,
   method,
   signal,
   ...rest,
 }).then((response) => {
   const contentType = response.headers.get("content-type");
   if (contentType && contentType.indexOf("json") !== -1) {
     if (response.status >= 400) {
       console.log("response >= 400 with json", response.statusText || response.status);
       return {
         status: response.status,
         message: response.statusText || response.status,
         response: response.json(),
       };
     }
     return response.json();
   }
   if (response.status >= 400) {
     console.log("response >= 400", response);
     return {
       status: response.status,
       message: response.statusText || response.status,
       response: response.text(),
     };
   }
   return response.text();
 });


 const cancel = () => {
   controller.abort();
 };


 // Store the request and cancel function for the endpoint
 if (key !== undefined) {
   activeRequests[key] = { request, cancel };
 }


 return request;
};