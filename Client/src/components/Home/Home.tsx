import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Navbar from "./Navbar/Navbar";
import Modules from "./Modules/Modules";
import Body from "./Body/Body";

import "./Home.css";
import { useEffect } from "react";

function Home() {
    useEffect(() => {
        const sessionToken = sessionStorage.getItem("token");
        const localToken = localStorage.getItem("token");
        if (!sessionToken && !localToken) {
            window.location.href = "/login";
        }
    }, []);
    return (
        <div className='student-page'>
            <Navbar />
            <Routes>
                <Route path="/modules" element={<Modules />} />
                <Route path="/profile" element={<Body />} />
                <Route path="/*" element={<Body />} />
            </Routes>
        </div>
    );
}

export default Home;