import { useAuth } from "../../contexts/AuthContext";
import StudentPage from "./Student/StudentPage";
import TeacherPage from "./Teacher/TeacherPage";

function Home() {
    const { user } = useAuth();
    return (
        <div>
            {
                user!.role === "teacher"
                    ? <TeacherPage />
                    : <StudentPage />
            }

        </div>
    );
}

export default Home;