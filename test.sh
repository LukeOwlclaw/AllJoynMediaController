SCRIPTPATH="$(pwd)/$(basename $0)"
me=`basename "$0"`
#echo $me
#echo $0
#echo $SCRIPTPATH
#MYECHO_TOTAL=$(grep -w echo -c $SCRIPTPATH)
MYECHO_TOTAL=$(grep "^[^#;]" $SCRIPTPATH | grep MYECHO_TOTAL -v | grep -c -w echo)

echo $MYECHO_TOTAL $MYECHO_TOTAL MYECHO_TOTAL

echo hallo