import React from "react";
import { BrowserRouter as Router, Route, Routes, Navigate } from "react-router-dom";
import Login from "./Pages/Login";

const AppRoutes = () => {
    return(
        <Router>
            <Routes>
                <Route path="/" element ={<Login />} />
            </Routes>
        </Router>
    )
}
export default AppRoutes