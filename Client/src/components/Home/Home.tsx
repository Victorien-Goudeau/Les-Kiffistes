import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import StudentPage from "./Student/StudentPage";
import TeacherPage from "./Teacher/TeacherPage";

function Home() {
    // const { user } = null;
    return (
        <div>
            {/* {
                user!.role === "teacher"
                    ? <TeacherPage />
                    : <StudentPage />
            } */}
            <Routes>
                <Route path="/" element={<StudentPage />} />
                <Route path="/student/*" element={<StudentPage />} />
                <Route path="/teacher/*" element={<TeacherPage />} />
            </Routes>
        </div>
    );
}

export default Home;