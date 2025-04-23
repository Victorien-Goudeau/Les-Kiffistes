import { Link } from 'react-router-dom';
import './Navbar.css';
import PersonIcon from '@mui/icons-material/Person';

function Navbar() {
    return (
        <div className="navbar">
            <Link to="/home/teacher" className="logo link">
                <h1>Teaching platform</h1>
            </Link>
            <div className="navbar-links">
                <Link to="/home/teacher/my-courses" className='link'>My Courses</Link>
                <Link to="/home/teacher/my-modules" className='link'>My Modules</Link>
            </div>
            <div className='profile'>
                <PersonIcon className='profile-icon' />
            </div>
        </div >
    );
}

export default Navbar;