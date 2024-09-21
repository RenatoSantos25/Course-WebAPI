import React from "react";
import { BrowserRouter as Router, Route, Routes, Navigate } from "react-router-dom";
import Login from "./Pages/Login";
import Home from "./Pages/home/home";

const AppRoutes = () => {
    return(
        <Router>
            <Routes>
                <Route path="/" element ={<Login />} />
                <Route path='/inicio' element={<Home />} />
            </Routes>
        </Router>
    )
}
export default AppRoutes