import ReactMarkdown from "react-markdown";
import "./Module.css";
import { useEffect, useState } from "react";
import { Link, Outlet, Route, Routes, useParams } from "react-router-dom";
import { useApi } from "../../../../customs/useApi";

interface Course {
    id: string;
    status: string;
    title?: string;
    subject?: string;
    content?: string;
    fileContent?: string;
}

interface AIModule {
    Id: string;
    Title: string;
    Content: string;
    Status: boolean;
}

function Module() {
    const [course, setCourse] = useState<Course | null>(null);
    const [modules, setModules] = useState<AIModule[] | null>(null);
    const [markdownText, setMarkdownText] = useState<string>("");
    const { id } = useParams<{ id: string }>();

    const { callApi } = useApi();

    useEffect(() => {
        callApi("GET", `Course/${id}`).then((response) => {
            return response.json();
        }).then((data) => {
            console.log("data:",data);
            setCourse(data);
            setMarkdownText(data.content);
        }
        ).catch((error) => {
            console.error("Erreur lors de la récupération du cours :", error);
        });

        callApi("GET", `Course/${id}/modules`).then((response) => {
            return response.json();
        }).then((data) => {
            console.log("modules:",data);
            setModules(data);
        }
        ).catch((error) => {
            console.error("Erreur lors de la récupération des modules :", error);
        });


    }, []);

    return (
        <div className="modules">
            <div className="sidebar">
                <div className="modules-list">
                    <h4>Remediation Modules</h4>
                    <Link to="#" onClick={() => setMarkdownText(course?.content || "")} className="module-link">
                        <div className="module-link-button">
                            Course
                        </div>
                    </Link>
                    {modules && modules.map((module) => (
                        <Link to="#" key={module.Id} onClick={() => setMarkdownText(module.Content)} className="module-link">
                            <div className={`module-link-button ${module.Status ? "completed" : ""}`}>
                                {module.Title}
                            </div>
                        </Link>
                    ))}
                </div>
            </div>
            <div className="modules-content">
                {course && (
                    <>
                        <h1>{course.title}</h1>
                        <ReactMarkdown>{markdownText}</ReactMarkdown>
                        <Link to={`/home/eval/${id}`} className="eval-link">
                            <div className="eval-button">
                                Start evaluation
                            </div>
                        </Link>
                    </>
                )}
            </div>
        </div>
    );
}

export default Module;