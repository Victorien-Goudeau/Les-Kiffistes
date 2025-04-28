import { useState } from "react";
import "./Register.css";
import { useApi } from "../../customs/useApi";

function Register() {
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const { callApi } = useApi();

    const handleRegister = () => {
        if (username === "" || email === "" || password === "") {
            return;
        }
        const body = JSON.stringify({
            username: username,
            email: email,
            password: password,
            role: "Student",
        });
        callApi("POST", "auth/register", body)
            .then((response) => {
                if (response.status === 200) {
                    return response.json();
                } else {
                    throw new Error("Registration failed");
                }
            })
            .then((data) => {
                localStorage.setItem("token", data.token);
                window.location.href = `/home`;
            })
            .catch((error) => {
                console.error("Error:", error);
            });
    };
    return (
        <div className="register-body">
            <div className="register-form">
                <h1>Register</h1>
                <div className="input-container">
                    <input type="text" placeholder="Username" className="register-input" onChange={(e) => setUsername(e.target.value)} />
                    <input type="email" placeholder="Email" className="register-input" onChange={(e) => setEmail(e.target.value)} />
                    <input type="password" placeholder="Password" className="register-input" onChange={(e) => setPassword(e.target.value)} />
                </div>
                <div className="register-button" onClick={handleRegister}>Sign up</div>
                <div className="login-container">
                    <p>Already have an account? <a href="/login" className="register-link">Sign in</a></p>
                </div>
            </div>
        </div>
    );
}

export default Register;