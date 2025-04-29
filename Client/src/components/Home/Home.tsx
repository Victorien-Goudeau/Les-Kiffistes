import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Navbar from "./Navbar/Navbar";
import Modules from "./Modules/Modules";
import Body from "./Body/Body";

import "./Home.css";
import { useEffect } from "react";
import Evaluation from "./Modules/Evaluation/Evaluation";

function Home() {
    useEffect(() => {
        const sessionToken = sessionStorage.getItem("token");
        const localToken = localStorage.getItem("token");

        console.log("Session Token:", sessionToken);
        console.log("Local Token:", localToken);
        if (!sessionToken && !localToken) {
            window.location.href = "/login";
        }
    }, []);
    return (
        <div className='student-page'>
            <Navbar />
            <Routes>
                <Route path="/modules/:id/*" element={<Modules />} />
                <Route path="/eval/:id" element={<Evaluation />} />
                <Route path="/profile" element={<Body />} />
                <Route path="/*" element={<Body />} />
            </Routes>
        </div>
    );
}

export default Home;