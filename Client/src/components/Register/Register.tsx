import "./Register.css";

function Register() {
    return (
        <div className="register-body">
            <div className="register-form">
                <h1>Register</h1>
                <div className="checkbox-wrapper-35">
                    <input value="private" name="switch" id="switch" type="checkbox" className="switch" />
                    <label htmlFor="switch">
                        <span className="switch-x-text">You are </span>
                        <span className="switch-x-toggletext">
                            <span className="switch-x-unchecked"><span className="switch-x-hiddenlabel">Unchecked: </span>student</span>
                            <span className="switch-x-checked"><span className="switch-x-hiddenlabel">Checked: </span>teacher</span>
                        </span>
                    </label>
                </div>
                <div className="input-container">
                    <input type="text" placeholder="Username" className="register-input" />
                    <input type="email" placeholder="Email" className="register-input" />
                    <input type="password" placeholder="Password" className="register-input" />
                </div>
                <div className="register-button">Sign up</div>
                <div className="login-container">
                    <p>Already have an account? <a href="/login" className="register-link">Sign in</a></p>
                </div>
            </div>
        </div>
    );
}

export default Register;