import { useState } from 'react';
import Navbar from './Navbar/Navbar';
import Body from './Body/Body';
import './StudentPage.css';
import { Routes, Route } from "react-router-dom";
import Modules from './Modules/Modules';
import Courses from './Courses/Courses';

function StudentPage() {
    return (
        <div className='student-page'>
            <Navbar />
            <Routes>
                <Route path="/course" element={<Courses />} />
                <Route path="/modules" element={<Modules />} />
                <Route path="/profile" element={<Body />} />
                <Route path="/*" element={<Body />} />
            </Routes>
        </div>
    );
}

export default StudentPage;