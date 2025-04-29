import { Link } from 'react-router-dom';
import './Body.css';
import { useApi } from '../../../customs/useApi';
import { useEffect, useState } from 'react';

function Body() {
    const { callApi } = useApi();
    const [courses, setCourses] = useState([]);

    useEffect(() => {
        callApi("GET", "Course/all").then((response) => {
            return response.json();
        }).then((data) => {
            console.log("Courses data:", data);
            setCourses(data);
        })
    }, []);
    return (
        <div className="body">
            {courses.map((course: any) => {
                return (
                    <div key={course.id} style={{ background: `#${Math.floor(Math.random() * 16777215).toString(16).padStart(6, "0")}` }} className="module">
                        <h1>{course.title}</h1>
                        <Link to={`/home/modules/${course.id}`}>
                            <div className='module-button'>
                                <p>Go to modules</p>
                            </div>
                        </Link>
                    </div>
                )
            })
            }
        </div >
    )
}

export default Body;