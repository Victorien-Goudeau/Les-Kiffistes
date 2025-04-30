import './App.css';
import Home from './components/Home/Home';
import Login from './components/Login/Login';
import Register from './components/Register/Register';
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Remediation from "./components/Home/Modules/Remediation/RemediationModules";

function App() {
  return (

    <div className="App">
      <Router>
        <Routes>
          <Route path="/" element={<Login />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/home/*" element={<Home />} />
          <Route path="/remediation" element={<Remediation />} />
        </Routes>
      </Router>
    </div>
  );
}

export default App;
