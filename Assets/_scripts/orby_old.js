#pragma strict

var force:float = 300;
var drag:float = 1;
var flag:int = 0; 

var screenPoint: Vector3;
var offset: Vector3;

var spot:GameObject;

function Start () {
	GetComponent.<Rigidbody2D>().drag = drag;
	
	//запоминаем пятно и выключаем
	spot = gameObject.Find("orby_spot");
	spot.SetActive(false);
			
}

function Update () {
	 if (GetComponent.<Rigidbody2D>().velocity.x == 0 && GetComponent.<Rigidbody2D>().velocity.y == 0 && flag == 1) gameObject.Find("helper").GetComponent.<GUIText>().text = "Game over";
  	
}
//нажимаем на шарик
function OnMouseDown()
{
    //screenPoint = Camera.main.WorldToScreenPoint(transform.position);
    screenPoint = transform.position;
    offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
}

//оттягиваем шарик
function OnMouseDrag () {
	var curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
    var curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
    transform.position = curPosition;
}

//отпускаем шарик, чтоб он полетел
function OnMouseUp () {
	var diff: Vector2;
	//diff = Vector2(0 , -3.5) - transform.position;
	diff = screenPoint - transform.position;
	GetComponent.<Rigidbody2D>().AddForce(new Vector2(diff.x * force, diff.y * force));
	flag = 1;
}

//ловим звезду или end_point
/*
function OnTriggerEnter2D(star: Collider2D) {
  if (star.gameObject.name == "end_point") {
  	rigidbody2D.drag = 20;
  	gameObject.Find("helper").guiText.text = "Victory";
  	flag = 0;
	return false;
  } else if (star.gameObject.name == "spot") {
  	renderer.material.color = Color.green;
  	//spot.SetActive(true);
  } else {
	Destroy(star.gameObject);
  }
}
*/