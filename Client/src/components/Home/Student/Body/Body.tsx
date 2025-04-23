import { Link } from 'react-router-dom';
import './Body.css';

function Body() {
    return (
        <div className="body">
            <div className="module">
                <h1>Mathematics</h1>
                <Link to="/home/student/course">
                    <div className='module-button'>
                        <p>Go to module</p>
                    </div>
                </Link>
            </div>
            <div style={{ background: "#FFCB77" }} className="module">
                <h1>English</h1>
                <Link to="/home/student/course">
                    <div className='module-button'>
                        <p>Go to module</p>
                    </div>
                </Link>
            </div>
            <div style={{ background: "#5C0029" }} className="module">
                <h1>Sciences</h1>
                <Link to="/home/student/course">
                    <div className='module-button'>
                        <p>Go to module</p>
                    </div>
                </Link>
            </div>
            <div style={{ background: "#EF9CDA" }} className="module">
                <h1>Geography</h1>
                <Link to="/home/student/course">
                    <div className='module-button'>
                        <p>Go to module</p>
                    </div>
                </Link>
            </div>
            <div style={{ background: "#DAA49A" }} className="module">
                <h1>Literacy</h1>
                <Link to="/home/student/course">
                    <div className='module-button'>
                        <p>Go to module</p>
                    </div>
                </Link>
            </div>
            <div style={{ background: "#E4572E" }} className="module">
                <h1>Philosophy</h1>
                <Link to="/home/student/course">
                    <div className='module-button'>
                        <p>Go to module</p>
                    </div>
                </Link>
            </div>
        </div>
    )
}

export default Body;