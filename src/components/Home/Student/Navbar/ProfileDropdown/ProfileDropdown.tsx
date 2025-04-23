import { useState } from "react";
import "./ProfileDropdown.css";
import PersonIcon from '@mui/icons-material/Person';

function ProfileDropdown() {
    const [isOpen, setIsOpen] = useState(false);
    return (
        <div className='profile' onClick={() => setIsOpen(!isOpen)} >
            <div className="dropdown">
                <PersonIcon className='profile-icon' />
                <div className={`dropdown-menu ${isOpen == true ? "show" : "dont-show"}`} onMouseLeave={() => setIsOpen(false)}>
                    <div className="buttons-container">
                        <div className="dropdown-button">Profile</div>
                        <div className="dropdown-button">Settings</div>
                        <div className="dropdown-button">Logout</div>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default ProfileDropdown;