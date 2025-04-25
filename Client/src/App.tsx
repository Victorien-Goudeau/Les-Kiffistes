import './App.css';
import Home from './components/Home/Home';
import Courses from './components/Home/Student/Courses/Courses';
import StudentPage from './components/Home/Student/StudentPage';
import TeacherPage from './components/Home/Teacher/TeacherPage';
import Login from './components/Login/Login';
import Register from './components/Register/Register';
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";

function App() {
  return (

    <div className="App">
      {/* {user ? (
        <Home />
      ) : (
        <Login />
      )} */}
      <Router>
        <Routes>
          <Route path="/" element={<Login />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/home/*" element={<Home />}>
            <Route path="student" element={<StudentPage />}>
              <Route path="my-courses" element={<Courses />} />
              <Route path="my-modules" element={<StudentPage />} />
              <Route path="profile" element={<StudentPage />} />
              <Route path="*" element={<StudentPage />} />
            </Route>
            <Route path="teacher/*" element={<TeacherPage />} />
          </Route>
        </Routes>
      </Router>
    </div>
  );
}

export default App;
