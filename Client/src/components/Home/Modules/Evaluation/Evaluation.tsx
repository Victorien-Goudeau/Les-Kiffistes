import { use, useEffect, useState } from "react";
import "./Evaluation.css";
import { useApi } from "../../../../customs/useApi";
import { useParams } from "react-router-dom";

interface Question {
    id: string;
    content: string;
    choices: string;
    correctAnswers: string;
    isUserAnswerCorrectly: boolean;
}

interface Quiz {
    id: string;
    title: string;
    questions: Question[];
}

function Evaluation() {
    const { callApi } = useApi();
    const { id } = useParams<{ id: string }>();

    const [quiz, setQuiz] = useState<Quiz | null>(null);
    const [questionIndex, setQuestionIndex] = useState(0);
    const [isQuizFinished, setIsQuizFinished] = useState(false);
    const [userAnswers, setUserAnswers] = useState<string[]>([]);

    useEffect(() => {
        console.log("ID:", id);
        callApi("GET", `quiz/${id}`).then((response) => {
            return response.json();
        }).then((data) => {
            console.log("Quiz data:", data);
            setQuiz(data);
        });
    }, []);

    const handleNextQuestion = () => {
        const selectedOption = document.querySelector(`input[name="question${questionIndex}"]:checked`) as HTMLInputElement;
        if (!selectedOption) {
            return;
        }
        if (selectedOption) {
            setUserAnswers((prevAnswers) => [...prevAnswers, selectedOption.value]);
        }
        setQuiz((prevQuiz) => {
            if (!prevQuiz) return null;
            const updatedQuestions = prevQuiz.questions.map((question, index) => {
                if (index === questionIndex) {
                    console.log("Selected option:", selectedOption.value);
                    console.log("Correct answer:", question.correctAnswers);
                    console.log("Is user answer correct:", selectedOption.value === question.correctAnswers);
                    return { ...question, isUserAnswerCorrectly: selectedOption.value === question.correctAnswers };
                }
                return question;
            });
            return { ...prevQuiz, questions: updatedQuestions };
        });
        selectedOption.checked = false; // Uncheck the selected option
        if (questionIndex < quiz!.questions.length - 2) {
            setQuestionIndex(questionIndex + 1);
        } else {
            setIsQuizFinished(true);
        }
    };

    return (
        <div className="evaluation-module">
            <h1>Evaluation</h1>
            <div className="question-container">
                <div className="question-container-header">
                    <h2>Question {questionIndex + 1}</h2>
                </div>
                <div className="question-container-body">
                    <p>{quiz?.questions[questionIndex].content}</p>
                    <div className="responses-container">
                        {quiz?.questions[questionIndex].choices.split("|").map((choice, index) => {
                            return (
                                <div className="response-option" key={index}>
                                    <input type="radio" id={`option${index}`} name={`question${questionIndex}`} value={choice} />
                                    <label htmlFor={`option${index}`} className="option-label">{choice}</label>
                                </div>
                            );
                        })}
                    </div>
                </div>
                <div className="question-container-footer">
                    <button className="submit-button" onClick={handleNextQuestion}>{isQuizFinished ? 'Finish' : 'Next'}</button>
                </div>
            </div>
        </div>
    );
}

export default Evaluation;