import { useState } from "react";
import "./Register.css";
import { useApi } from "../../customs/useApi";

function Register() {
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [role, setRole] = useState("Student");

    const { callApi } = useApi();

    const handleSwitchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const isChecked = event.target.checked;
        setRole(isChecked ? "Teacher" : "Student");
    };

    const handleRegister = () => {
        if (username === "" || email === "" || password === "") {
            return;
        }
        const body = JSON.stringify({
            username: username,
            email: email,
            password: password,
            role: role,
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
                window.location.href = `/home/${role.toLowerCase()}`;
            })
            .catch((error) => {
                console.error("Error:", error);
            });
    };
    return (
        <div className="register-body">
            <div className="register-form">
                <h1>Register</h1>
                <div className="checkbox-wrapper-35">
                    <input value="private" name="switch" id="switch" type="checkbox" className="switch" onChange={handleSwitchChange} />
                    <label htmlFor="switch">
                        <span className="switch-x-text">You are </span>
                        <span className="switch-x-toggletext">
                            <span className="switch-x-unchecked"><span className="switch-x-hiddenlabel">Unchecked: </span>student</span>
                            <span className="switch-x-checked"><span className="switch-x-hiddenlabel">Checked: </span>teacher</span>
                        </span>
                    </label>
                </div>
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