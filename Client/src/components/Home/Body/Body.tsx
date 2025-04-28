import { Link } from 'react-router-dom';
import './Body.css';

function Body() {
    const moduleId = 1; // This should be dynamic based on the module selected
    return (
        <div className="body">
            <div className="module">
                <h1>Course 1</h1>
                <Link to={`/home/modules/${moduleId}`}>
                    <div className='module-button'>
                        <p>Go to modules</p>
                    </div>
                </Link>
            </div>
            <div style={{ background: "#FFCB77" }} className="module">
                <h1>Course 2</h1>
                <Link to={`/home/modules/${moduleId}`}>
                    <div className='module-button'>
                        <p>Go to modules</p>
                    </div>
                </Link>
            </div>
            <div style={{ background: "#5C0029" }} className="module">
                <h1>Course 3</h1>
                <Link to={`/home/modules/${moduleId}`}>
                    <div className='module-button'>
                        <p>Go to modules</p>
                    </div>
                </Link>
            </div>
            <div style={{ background: "#EF9CDA" }} className="module">
                <h1>Course 4</h1>
                <Link to={`/home/modules/${moduleId}`}>
                    <div className='module-button'>
                        <p>Go to modules</p>
                    </div>
                </Link>
            </div>
            <div style={{ background: "#DAA49A" }} className="module">
                <h1>Course 5</h1>
                <Link to={`/home/modules/${moduleId}`}>
                    <div className='module-button'>
                        <p>Go to modules</p>
                    </div>
                </Link>
            </div>
            <div style={{ background: "#E4572E" }} className="module">
                <h1>Course 6</h1>
                <Link to={`/home/modules/${moduleId}`}>
                    <div className='module-button'>
                        <p>Go to modules</p>
                    </div>
                </Link>
            </div>
        </div >
    )
}

export default Body;