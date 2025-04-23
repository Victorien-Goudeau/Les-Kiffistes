import { Link } from 'react-router-dom';
import './Navbar.css';
import ProfileDropdown from './ProfileDropdown/ProfileDropdown';

function Navbar() {
    return (
        <div className="navbar">
            <Link to="/home/student" className="logo link">
                <h1>Teaching platform</h1>
            </Link>
            <div className="navbar-links">
                <Link to="/home/student/my-courses" className='link'>My Courses</Link>
                <Link to="/home/student/my-modules" className='link'>My Modules</Link>
            </div>
            <ProfileDropdown />
        </div >
    );
}

export default Navbar;