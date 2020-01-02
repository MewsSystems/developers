import React from "react";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.min.css";

const CustomAlert = () => {
  return (
    <ToastContainer
      position="top-left"
      autoClose={false}
      newestOnTop={false}
      closeOnClick
      rtl={false}
      draggable
    />
  );
};

export default CustomAlert;
