import "./Login.css"

function Login() {
    return (
        <div className="login-body">
            <div className="login-form">
                <h1>Login</h1>
                <div className="input-container">
                    <input type="text" placeholder="Username" className="login-input" />
                    <input type="password" placeholder="Password" className="login-input" />
                </div>
                <div className="options-container">
                    <div className="remember-me-container">
                        <input type="checkbox" id="remember-me" className="remember-me-checkbox" />
                        <label htmlFor="remember-me" className="remember-me-label">Remember Me</label>
                    </div>
                    <a href="#" className="forgot-password">Forgot Password?</a>
                </div>
                <div className="login-button">Login</div>
                <div className="register-container">
                    <p>Don't have an account? <a href="/register" className="register-link">Register</a></p>
                </div>
            </div>
        </div>
    );
}

export default Login;