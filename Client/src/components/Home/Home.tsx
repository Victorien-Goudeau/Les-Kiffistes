import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";
import StudentPage from "./Student/StudentPage";
import TeacherPage from "./Teacher/TeacherPage";

function Home() {
    const { user } = useAuth();
    return (
        <div>
            {/* {
                user!.role === "teacher"
                    ? <TeacherPage />
                    : <StudentPage />
            } */}
            <Routes>
                <Route path="/" element={user ? <TeacherPage /> : <StudentPage />} />
                <Route path="/student/*" element={<StudentPage />} />
                <Route path="/teacher/*" element={<TeacherPage />} />
            </Routes>
        </div>
    );
}

export default Home;