import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Navbar from "./Navbar/Navbar";
import Module from "./Modules/Module/Module";
import Body from "./Body/Body";

import "./Home.css";
import { useEffect } from "react";
import Evaluation from "./Modules/Evaluation/Evaluation";
import Remediation from "./Modules/Remediation/RemediationModules";

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
                <Route path="/modules/:id/*" element={<Module />} />
                <Route path="/eval/:id/*" element={<Evaluation />} />
                {/* <Route path="/remediation" element={<Remediation />} /> */}
                <Route path="/profile" element={<Body />} />
                <Route path="/*" element={<Body />} />
            </Routes>
        </div>
    );
}

export default Home;