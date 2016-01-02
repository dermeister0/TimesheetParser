/*var svg2png = require('svg2png');

function generateImage(fileName, size) {
    var sourceSize = 96.0;
    var scale = size / sourceSize;
    
    svg2png('../../Images/Icon00.svg', fileName, scale, function (err) { });
}

generateImage('Test.png', 128);
*/

var gm = require('gm');
var fs = require('fs');

var tempCounter = 0;

function generateImage(fileName, width, height, transparent) {
    var tempFile = 'temp' + ++tempCounter + '.png';
    var iconSize = height * 0.66;
    
    gm('../../Images/Icon00.svg')
        .background('none')
        .density(600, 600)
        .negative()
        .resize(iconSize)
        .write(tempFile, function (err) {
            if (err) {
                console.log(err);
                return;
            }
            
            var offsetX = (width - iconSize) / 2;
            var offsetY = (height - iconSize) / 2;
            
            var img = gm(tempFile);
            
            if (transparent) {
                img.background('none');
            } else {
                img.background('#1593AC')
            }
                
            img.extent(width, height, ` -${offsetX} -${offsetY}`)
                .write(fileName, function (err) {
                    if (err) {
                        console.log(err);
                    }
                    
                    console.log(`Updated ${fileName}.`);
                    fs.unlink(tempFile);
                });
        });
}

var outputDir = '../TimesheetParser.Win10/Assets/';

generateImage(outputDir + 'LockScreenLogo.scale-200.png', 48, 48, false);
generateImage(outputDir + 'SplashScreen.scale-200.png', 1240, 600, true);
generateImage(outputDir + 'Square44x44Logo.scale-200.png', 88, 88, false);
generateImage(outputDir + 'Square44x44Logo.targetsize-24_altform-unplated.png', 24, 24, false);
generateImage(outputDir + 'Square150x150Logo.scale-200.png', 300, 300, true);
generateImage(outputDir + 'StoreLogo.png', 50, 50, false);
generateImage(outputDir + 'Wide310x150Logo.scale-200.png', 620, 300, true);
