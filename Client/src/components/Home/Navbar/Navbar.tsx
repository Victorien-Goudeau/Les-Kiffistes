import { Link, useLocation } from 'react-router-dom';
import './Navbar.css';
import ProfileDropdown from './ProfileDropdown/ProfileDropdown';
import AddFileComponent from './AddFileComponent/AddFileComponent';

function Navbar() {
    const location = useLocation();
    return (
        <div className="navbar">
            <Link to="/home" className="logo link">
                <h1>Teaching platform</h1>
            </Link>
            <div className="navbar-actions">
                {location.pathname === "/home" && (
                    <AddFileComponent />
                )}
                <ProfileDropdown />
            </div>
        </div >
    );
}

export default Navbar;