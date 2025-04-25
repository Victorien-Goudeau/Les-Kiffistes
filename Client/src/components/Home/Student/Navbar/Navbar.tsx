import { Link, useLocation } from 'react-router-dom';
import './Navbar.css';
import ProfileDropdown from './ProfileDropdown/ProfileDropdown';

function Navbar() {
    const location = useLocation();
    return (
        <div className="navbar">
            <Link to="/home/student" className="logo link">
                <h1>Teaching platform</h1>
            </Link>
            <div className={`navbar-actions ${location.pathname === "/home/student" ? "with-button" : ""}`}>
                {location.pathname === "/home/student" && (
                    <label htmlFor="file-upload" className="custom-file-upload">
                        <input id="file-upload" type="file" className="add-course-button" />
                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" className="feather feather-plus-circle"><circle cx="12" cy="12" r="10"></circle><line x1="12" y1="8" x2="12" y2="16"></line><line x1="8" y1="12" x2="16" y2="12"></line></svg>
                        <span className="add-course-text">Add Course</span>
                    </label>
                )}
                <ProfileDropdown />
            </div>
        </div >
    );
}

export default Navbar;