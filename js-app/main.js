const url = "https://localhost:5001/api/beanvariety/";

const button = document.querySelector("#run-button");
button.addEventListener("click", () => {
    getAllBeanVarieties()
});

function getAllBeanVarieties() {
    return fetch(url).then(res => res.json())
    .then(bv => {
        bv.map(bv => {
            document.getElementById("types").innerHTML += `<h1> ${bv.name} </h1> <p>Region: ${bv.region}</div> <div> Notes: ${bv.notes ? bv.notes : "None"}</p>`
        })
    })
}

