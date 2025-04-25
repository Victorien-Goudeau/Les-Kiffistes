import { useRef, useState } from "react";
import "./Login.css"
import { useApi } from "../../customs/useApi";

function Login() {
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState(false);

    const { callApi } = useApi();

    const rememberMeRef = useRef<HTMLInputElement>(null);

    const handleLogin = () => {
        if (login === "" || password === "") {
            return;
        }
        const body = JSON.stringify({
            login: login,
            password: password,
        });
        callApi("POST", "auth/login", body)
            .then((response) => {
                if (response.status === 200) {
                    return response.json();
                } else {
                    throw new Error("Login failed");
                }
            })
            .then((data) => {
                setError(false);
                if (rememberMeRef.current?.checked) {
                    localStorage.setItem("token", data.accessToken);
                } else {
                    sessionStorage.setItem("token", data.accessToken);
                }
                window.location.href = "/home";
            })
            .catch((error) => {
                setError(true);
                console.error("Error:", error);
            });
    }

    return (
        <div className="login-body">
            <div className="login-form">
                <h1>Login</h1>
                {error && <p style={{ position: 'absolute', bottom: '40%', color: 'red' }}>Error : wrong username or password.</p>}
                <div className="input-container">
                    <input type="text" placeholder="Username" className="login-input" onChange={(e) => setLogin(e.target.value)} />
                    <input type="password" placeholder="Password" className="login-input" onChange={(e) => setPassword(e.target.value)} />
                </div>
                <div className="options-container">
                    <div className="remember-me-container">
                        <input ref={rememberMeRef} type="checkbox" id="remember-me" className="remember-me-checkbox" />
                        <label htmlFor="remember-me" className="remember-me-label">Remember Me</label>
                    </div>
                    <a href="#" className="forgot-password">Forgot Password?</a>
                </div>
                <div className="login-button" onClick={handleLogin}>Login</div>
                <div className="register-container">
                    <p>Don't have an account? <a href="/register" className="register-link">Register</a></p>
                </div>
            </div>
        </div>
    );
}

export default Login;